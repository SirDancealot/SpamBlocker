using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using NetFwTypeLib;
using SpamBlocker.program.data.IP;
using SpamBlocker.program.ui;

namespace SpamBlocker.program.logic
{
    class FirewallManager
    {
        private static readonly string ruleName = ConfigurationManager.AppSettings.Get("fwRuleName");

        public static void BlockIPs(Dictionary<string, IP>.ValueCollection ips)
        {
            bool newRule = false;
            bool noThreats = true;
            Logger l = Logger.getINSTANCE();
            INetFwPolicy2 fwPolicy2 = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            INetFwRule _rule = null;
            foreach (INetFwRule rule in fwPolicy2.Rules)
            {
                if (rule.Name.Equals(ruleName))
                {
                    _rule = rule;
                }
            }
            if (_rule == null)
            {
                newRule = true;
                _rule = (INetFwRule2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                _rule.Enabled = true;
                _rule.Name = ruleName;
                _rule.Action = NET_FW_ACTION_.NET_FW_ACTION_BLOCK;
                _rule.Description = "Rule blocking spam attempts from frequently trying IP-addresses";
                _rule.Profiles = 0b111;
            }

            StringBuilder sb = new StringBuilder();

            if (_rule.RemoteAddresses.Length != 0 && !_rule.RemoteAddresses.Equals("*"))
            {
                sb.Append(_rule.RemoteAddresses).Append(',');
            }

            foreach (IP ip in ips)
            {
                if (ip.Count >= ip.DangerCount)
                {
                    noThreats = false;
                    sb.Append(ip).Append(',');
                    l.logIP(ip);
                }

            }
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            if (newRule && !noThreats)
                fwPolicy2.Rules.Add(_rule);

            _rule.RemoteAddresses = sb.ToString();
        }
    }
}

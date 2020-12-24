using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC.Workflows.WFC.Operators
{
    public class OrangeWorkflowConfig
    {
        public string InvoiceFileName { get; set; }
        public string UsageFileName { get; set; }
        public int? BillingIntegrationRetryWindow { get; set; } // In Days
        public bool EnableAssetIntegration { get; set; }
        public bool AutoCreateAssets { get; set; }
        public bool AutoDeleteAssets { get; set; }
        public bool AutoSyncSimCards { get; set; }
    }
}

using System.Collections.Generic;

namespace SignalBox.Azure
{
    public static class AzureUsers
    {
        public static IList<AzureSynapseUser> AzureUserList = new List<AzureSynapseUser>
        {
            new AzureSynapseUser
            {
                Name = "Isaac",
                PrincipalId = "b689c422-6685-467a-97d2-8e34d5578571",
                PrincipalType = "User"
            },
            new AzureSynapseUser
            {
                Name = "Rian",
                PrincipalId = "16ab5eb9-f9e7-4df0-b40c-58ad0e0022d8",
                PrincipalType = "User"
            },
            new AzureSynapseUser
            {
                Name = "Emil",
                PrincipalId = "61e8583a-4b23-42f9-a951-6b90686c8410",
                PrincipalType = "User"
            },
            new AzureSynapseUser
            {
                Name = "azure-devops-dev",
                PrincipalId = "aacb14dc-7cbc-48b0-8b24-3a99aff7a3b5",
                PrincipalType = "ServicePrincipal"
            },
            new AzureSynapseUser
            {
                Name = "AzureDevopsProdApp",
                PrincipalId = "886c22ab-16af-47be-9fe6-b7dc44e7f200",
                PrincipalType = "ServicePrincipal"
            },
        };
    }

    public struct AzureSynapseUser
    {
        public string Name { get; set; }
        public string PrincipalId { get; set; }
        public string PrincipalType { get; set; }
    }
}

import React from "react";
import { CodeView } from "../../molecules/CodeView";

const jsIntegrate = ({ id }) => `
import { promotionsCampaigns } from "cherry.ai";

promotionsCampaigns.invokePromotionsCampaignAsync({
    token: "Your JSON Web Token / access token",
    id: ${id}, // the id of this campaign
    input: {
        commonUserId: "The common Id of the user to recommend for",
    }
})
.then( recommendation => console.log('success callback'))
.catch( error => alert('error callback') );
`;

const restIntegrate = ({ id, basePath }) => `
# location
POST ${basePath}/api/Campaigns/PromotionsCampaigns/${id}/Invoke

# headers
Content-Type: 'application/json'

# content
{
    "commonUserId": "User to recommend to" ,
    "arguments": {
      "basketSize": 5
    }
}
`;

export const JsIntegrate = ({ id }) => {
  return <CodeView language="js" text={jsIntegrate({ id })}></CodeView>;
};

export const RESTIntegrate = ({ id }) => {
  const origin = window.location.origin;
  return (
    <CodeView
      language="bash"
      text={restIntegrate({ id, basePath: origin })}
    ></CodeView>
  );
};

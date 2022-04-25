import React from "react";
import { Globe2, PlusCircleDotted } from "react-bootstrap-icons";
const hubspotIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/HubSpot-Inversed-Favicon.webp";
const segmentIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/Segment-Favicon.png";
const shopifyIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/Shopify-Bag.png";
const klaviyoIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/Klaviyo-Icon.png";

export const IntegrationIcon = ({ integration, systemType }) => {
  if (!systemType) {
    systemType = integration.systemType;
  }
  let src = null;
  switch (systemType?.toLowerCase()) {
    case "hubspot":
      src = hubspotIcon;
      break;
    case "segment":
      src = segmentIcon;
      break;
    case "shopify":
      src = shopifyIcon;
      break;
    case "klaviyo":
      src = klaviyoIcon;
      break;
    default:
      break;
  }
  if (src) {
    return (
      <img
        src={src}
        className="img-thumbnail"
        alt={`${systemType} Icon`}
        style={{ maxHeight: "200px" }}
      ></img>
    );
  } else if (systemType?.toLowerCase() === "custom") {
    return <PlusCircleDotted size={50} />;
  } else if (systemType?.toLowerCase() === "website") {
    return <Globe2 size={50} />;
  } else return <React.Fragment />;
};

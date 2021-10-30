import React from "react";
import { PlusCircleDotted } from "react-bootstrap-icons";
const hubspotIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/HubSpot-Inversed-Favicon.webp";
const segmentIcon =
  "https://docshostcce3f6dc.blob.core.windows.net/content/images/Segment-Favicon.png";

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
    default:
      break;
  }
  if (src) {
    return <img src={src} className="img-thumbnail" alt="Hubspot Icon"></img>;
  } else if (systemType?.toLowerCase() === "custom") {
    return <PlusCircleDotted size={50} />
  } else return <React.Fragment />;
};

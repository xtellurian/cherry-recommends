import React from "react";
import {
  MoreOptionsDropdown,
  OptionItem,
  OptionItemsGroup,
  OptionLink,
} from "../../molecules/MoreOptionsDropdown";

export const ActionsButtonUtil = ({
  basePath,
  setDeleteOpen,
  id,
  children,
}) => {
  return (
    <MoreOptionsDropdown>
      <OptionItemsGroup label="Actions">
        {/* <OptionLink to={`${basePath}/recommendations/${id}`}>
          Latest Recommendations
        </OptionLink> */}
        <OptionLink to={`${basePath}/monitor/${id}`}>Monitor</OptionLink>
        <OptionLink to={`${basePath}/test/${id}`}>Test</OptionLink>
        <OptionLink to={`${basePath}/settings/${id}`}>Settings</OptionLink>
        <OptionLink to={`${basePath}/destinations/${id}`}>
          Destinations
        </OptionLink>
        <OptionLink to={`${basePath}/triggers/${id}`}>Triggers</OptionLink>
        {/* <OptionLink to={`${basePath}/target-variable/${id}`}>
          Target Variable
        </OptionLink> */}
        <OptionLink to={`${basePath}/integrate/${id}`}>Integrate</OptionLink>
        {/* <OptionLink to={`${basePath}/invokation-logs/${id}`}>
          Invokation Logs
        </OptionLink> */}
        <OptionLink to={`${basePath}/link-to-model/${id}`}>
          Link to Model
        </OptionLink>{" "}
        <OptionItem onClick={() => setDeleteOpen(true)}>Delete</OptionItem>
      </OptionItemsGroup>
      {children}
    </MoreOptionsDropdown>
  );
  // return (
  //   <ActionsButton
  //     to={`${basePath}/recommendations/${id}`}
  //     label="Latest Recommendations"
  //   >
  //     <ActionItemsGroup label="Actions">
  //       <ActionLink to={`${basePath}/test/${id}`}>
  //         Recommender Testing
  //       </ActionLink>
  //       <ActionLink to={`${basePath}/settings/${id}`}>
  //         Settings
  //       </ActionLink>
  //       <ActionLink to={`${basePath}/target-variable/${id}`}>
  //         Target Variable
  //       </ActionLink>
  //       <ActionLink to={`${basePath}/integrate/${id}`}>
  //         Technical Integration
  //       </ActionLink>
  //       <ActionLink to={`${basePath}/invokation-logs/${id}`}>
  //         Invokation Logs
  //       </ActionLink>
  //       <ActionLink to={`${basePath}/link-to-model/${id}`}>
  //         Link to Model
  //       </ActionLink>
  //       <ActionItem onClick={() => setDeleteOpen(true)}>Delete</ActionItem>
  //     </ActionItemsGroup>
  //     {children}
  //   </ActionsButton>
  // );
};

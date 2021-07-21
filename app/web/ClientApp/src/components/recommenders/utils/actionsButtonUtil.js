import React from "react";
import {
  ActionsButton,
  ActionItem,
  ActionItemsGroup,
  ActionLink,
} from "../../molecules/ActionsButton";

export const ActionsButtonUtil = ({
  basePath,
  setDeleteOpen,
  id,
  children,
}) => {
  return (
    <ActionsButton
      to={`${basePath}/recommendations/${id}`}
      label="Latest Recommendations"
    >
      <ActionItemsGroup label="Actions">
        <ActionLink to={`${basePath}/test/${id}`}>
          Recommender Testing
        </ActionLink>
        <ActionLink to={`${basePath}/target-variable/${id}`}>
          Target Variable
        </ActionLink>
        <ActionLink to={`${basePath}/integrate/${id}`}>
          Technical Integration
        </ActionLink>
        <ActionLink to={`${basePath}/invokation-logs/${id}`}>
          Invokation Logs
        </ActionLink>
        <ActionLink to={`${basePath}/link-to-model/${id}`}>
          Link to Model
        </ActionLink>
        <ActionItem onClick={() => setDeleteOpen(true)}>Delete</ActionItem>
      </ActionItemsGroup>
      {children}
    </ActionsButton>
  );
};

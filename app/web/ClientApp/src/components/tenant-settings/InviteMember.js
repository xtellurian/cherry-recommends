import React from "react";
import { useAccessToken } from "../../api-hooks/token";
import { createTenantMembershipAsync } from "../../api/tenantsApi";
import { AsyncButton, ErrorCard } from "../molecules";
import { EntityRow } from "../molecules/layout/EntityRow";
import {
  InputGroup,
  TextInput,
  joinValidators,
  emailValidator,
  createRequiredByServerValidator,
} from "../molecules/TextInput";

function delay(ms) {
  return new Promise((resolve) => setTimeout(resolve, ms));
}

export const InviteMemberSection = ({ onNewMemberAdded }) => {
  const [email, setEmail] = React.useState("");
  const [loading, setLoading] = React.useState(false);
  const [error, setError] = React.useState();
  const token = useAccessToken();
  const handleInvite = () => {
    setLoading(true);
    createTenantMembershipAsync({ token, email })
      .then(onNewMemberAdded)
      .catch(setError)
      .finally(() => setLoading(false));
  };
  return (
    <>
      <EntityRow>
        <div className="col">
          <h5>Add Member</h5>
          {error && <ErrorCard error={error} />}
          <InputGroup>
            <TextInput
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              placeholder="wendy@example.com"
              validator={joinValidators([
                emailValidator,
                createRequiredByServerValidator(error),
              ])}
              onReturn={handleInvite}
            />
            <AsyncButton
              loading={loading}
              disabled={email.length < 3}
              className="btn btn-primary"
              onClick={handleInvite}
            >
              Invite
            </AsyncButton>
          </InputGroup>
        </div>
      </EntityRow>
    </>
  );
};
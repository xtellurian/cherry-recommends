import React from "react";
import { Spinner, Title } from "../molecules";
import { BigPopup } from "../molecules/popups/BigPopup";
import { TenantMembersSection } from "./TenantMembers";
import { InviteMemberSection } from "./InviteMember";
import { CopyableField } from "../molecules/fields/CopyableField";
import { TenantBillingSection } from "./TenantBillingSection";

export const TenantSettingsSummary = () => {
  const [loading, setLoading] = React.useState(false);
  const [newUserInfo, setNewUserInfo] = React.useState();
  const [newUserInvitatonPopupOpen, setNewUserInvitationPopupOpen] =
    React.useState(false);
  const handleNewMembderAdded = (s) => {
    setLoading(true);
    setNewUserInvitationPopupOpen(true);
    setNewUserInfo(s);
  };

  React.useEffect(() => {
    if (!newUserInfo) {
      setLoading(false);
    }
  }, [newUserInfo]);
  return (
    <>
      <div>
        <Title> Tenant Settings </Title>
        <hr />

        {loading ? <Spinner /> : <TenantBillingSection />}
        {loading ? (
          <Spinner />
        ) : (
          <TenantMembersSection>
            <div className="mt-4">
              <InviteMemberSection onNewMemberAdded={handleNewMembderAdded} />
            </div>
          </TenantMembersSection>
        )}
        {loading && <Spinner />}
      </div>

      {newUserInfo && (
        <BigPopup
          isOpen={newUserInvitatonPopupOpen}
          setIsOpen={setNewUserInvitationPopupOpen}
        >
          <div style={{ minHeight: "50vh" }} className="pt-5">
            <h5>New User Added</h5>
            <p>
              Send this link to your new team member to set up their account.
            </p>

            <CopyableField label="Link" value={newUserInfo.invitationUrl} />

            <div className="mt-5">
              <button
                className="btn btn-primary btn-block"
                onClick={() => {
                  setNewUserInvitationPopupOpen(false);
                  setNewUserInfo(null);
                }}
              >
                I've sent them the link
              </button>
            </div>
          </div>
        </BigPopup>
      )}
    </>
  );
};

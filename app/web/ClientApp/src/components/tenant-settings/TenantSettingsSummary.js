import React from "react";
import { Title } from "../molecules";
import { SmallPopup } from "../molecules/popups/SmallPopup";
import { TenantMembersSection } from "./TenantMembers";
import { InviteMemberSection } from "./InviteMember";
import { TenantBillingSection } from "./TenantBillingSection";
import { TenantInfoSection } from "./TenantInfoSection";

export const TenantSettingsSummary = () => {
  const [popupOpen, setPopupOpen] = React.useState(false);
  return (
    <>
      <div>
        <Title> Tenant Settings </Title>
        <hr />

        <TenantInfoSection />
        <TenantBillingSection />

        <TenantMembersSection>
          <div className="mt-4">
            <InviteMemberSection onNewMemberAdded={() => setPopupOpen(true)} />
            <SmallPopup isOpen={popupOpen} setIsOpen={setPopupOpen}>
              <div className="text-center p-3">
                <div className="mb-3">
                  We've sent an email inviting your new team member.
                </div>
                <button
                  className="btn btn-primary m-auto"
                  onClick={() => setPopupOpen(false)}
                >
                  Close
                </button>
              </div>
            </SmallPopup>
          </div>
        </TenantMembersSection>
      </div>
    </>
  );
};

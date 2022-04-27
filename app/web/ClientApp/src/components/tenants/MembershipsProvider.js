import React from "react";
import { useMemberships as useMembershipsHook } from "../../api-hooks/tenantsApi";

export const MembershipsContext = React.createContext();

export const useMemberships = () => React.useContext(MembershipsContext);

export const MembershipsProvider = ({ children }) => {
  const membershipsFromHook = useMembershipsHook();
  const [memberships, setMemberships] = React.useState([]);

  React.useEffect(() => {
    setMemberships(membershipsFromHook);
  }, [membershipsFromHook]);

  return (
    <MembershipsContext.Provider value={memberships}>
      {children}
    </MembershipsContext.Provider>
  );
};

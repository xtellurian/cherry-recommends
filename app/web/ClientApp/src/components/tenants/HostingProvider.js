import React from "react";
import { useHosting as useHostingApiHook } from "../../api-hooks/reactConfigApi";
import { PathTenantContext } from "./PathTenantProvider";
import { MembershipsContext } from "./MembershipsProvider";

export const HostingContext = React.createContext();

export const useHosting = () => React.useContext(HostingContext);

export const HostingProvider = ({ children }) => {
  const hostingFromHook = useHostingApiHook();
  const [hosting, setHosting] = React.useState(null);

  React.useEffect(() => {
    setHosting(hostingFromHook);
  }, [hostingFromHook]);

  return (
    <HostingContext.Provider value={hosting}>
      <PathTenantContext.Provider value={{ tenantName: "" }}>
        <MembershipsContext.Provider value={[]}>
          {children}
        </MembershipsContext.Provider>
      </PathTenantContext.Provider>
    </HostingContext.Provider>
  );
};

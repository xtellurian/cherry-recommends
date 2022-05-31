import React from "react";
import { useParams } from "react-router-dom";
import { setTenant } from "cherry.ai";

export const PathTenantContext = React.createContext();

export const useTenantName = () => React.useContext(PathTenantContext);

export const PathTenantProvider = ({ children }) => {
  const params = useParams();
  const [tenantName, setTenantName] = React.useState(null);

  if (params.tenant) {
    if (params.tenant !== tenantName) {
      setTenantName(params.tenant);
    }
  } else if (tenantName) {
    setTenantName();
  }

  React.useEffect(() => {
    console.debug(`Setting tenant name = ${tenantName} (by pathname)`);
    setTenant(tenantName);
  }, [tenantName]);

  return (
    <PathTenantContext.Provider value={{ tenantName, setTenantName }}>
      {children}
    </PathTenantContext.Provider>
  );
};

import { useAuth0 } from "@auth0/auth0-react";

export const useAuth = () => {
  const { isLoading, isAuthenticated } = useAuth0();
  return {
    isLoading,
    isAuthenticated,
  };
};

import { useAuth0 } from "@auth0/auth0-react";

export const useAuth = () => {
  const { user, isLoading, isAuthenticated, logout } = useAuth0();
  return {
    user,
    isLoading,
    isAuthenticated,
    logout
  };
};

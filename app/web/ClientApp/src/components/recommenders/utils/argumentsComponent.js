import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { AsyncButton, EmptyList, ErrorCard, Spinner } from "../../molecules";
import { ArgumentsEditor } from "../../molecules/ArgumentsEditor";
export const ArgumentsComponentUtil = ({
  id,
  useArguments,
  setArgumentsAsync,
}) => {
  const token = useAccessToken();
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();
  const [trigger, setTrigger] = React.useState({});
  const serverArgs = useArguments({ id, trigger });

  const [internalArgs, setInternalArgs] = React.useState();

  React.useEffect(() => {
    if (Array.isArray(serverArgs)) {
      setInternalArgs(serverArgs);
    }
  }, [serverArgs]);

  const setArgs = () => {
    setSaving(true);
    setError(null);
    setArgumentsAsync({ id, token, args: internalArgs })
      .catch(setError)
      .finally(() => {
        setSaving(false);
        setTrigger({});
      });
  };

  return (
    <React.Fragment>
      <div className="mb-2">
        {internalArgs && internalArgs.length === 0 ? (
          <EmptyList>There are no arguments yet.</EmptyList>
        ) : null}

        {/* <Typography className="semi-bold">Edit Arguments</Typography> */}
        {error ? <ErrorCard error={error} /> : null}
        {!serverArgs.loading && internalArgs ? (
          <ArgumentsEditor
            initialArguments={internalArgs}
            onArgumentsChanged={setInternalArgs}
          />
        ) : null}
      </div>
      {!internalArgs ? <Spinner /> : null}
      <AsyncButton
        className="btn btn-primary w-100 mt-4"
        loading={saving}
        onClick={setArgs}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

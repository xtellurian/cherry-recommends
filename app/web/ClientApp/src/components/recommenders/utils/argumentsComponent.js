import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { AsyncButton, ErrorCard, Spinner } from "../../molecules";
import { ArgumentsEditor } from "../../molecules/ArgumentsEditor";
export const ArgumentsComponentUtil = ({ recommender, setArgumentsAsync }) => {
  const token = useAccessToken();
  const initialArgs = recommender.arguments || [];
  const [args, setArguments] = React.useState(initialArgs);
  const [saving, setSaving] = React.useState(false);
  const [error, setError] = React.useState();

  React.useEffect(() => {
    if (recommender.arguments) {
      setArguments(recommender.arguments);
    }
  }, [recommender]);

  const setArgs = () => {
    setSaving(true);
    setError(null);
    setArgumentsAsync({ id: recommender.id, token, args })
      .catch(setError)
      .finally(() => setSaving(false));
  };

  return (
    <React.Fragment>
      <div className="mb-2">
        <h5>Edit Arguments</h5>
        {error && <ErrorCard error={error} />}
        {args && (
          <ArgumentsEditor
            initialArguments={initialArgs}
            onArgumentsChanged={setArguments}
          />
        )}
      </div>
      {!args && <Spinner />}
      <AsyncButton className="btn btn-primary w-25" loading={saving} onClick={setArgs}>
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

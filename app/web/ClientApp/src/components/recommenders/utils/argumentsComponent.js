import React from "react";
import { useAccessToken } from "../../../api-hooks/token";
import { AsyncButton, EmptyList, ErrorCard, Spinner } from "../../molecules";
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
        {args.length === 0 ? (
          <EmptyList>There are no arguments yet.</EmptyList>
        ) : null}

        {/* <Typography className="semi-bold">Edit Arguments</Typography> */}
        {error ? <ErrorCard error={error} /> : null}
        {args ? (
          <ArgumentsEditor
            initialArguments={initialArgs}
            onArgumentsChanged={setArguments}
          />
        ) : null}
      </div>
      {!args ? <Spinner /> : null}
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

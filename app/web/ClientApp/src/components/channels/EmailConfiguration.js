import React, { useState, useEffect } from "react";
import { useAccessToken } from "../../api-hooks/token";
import { updateEmailChannelTriggerAsync } from "../../api/channelsApi";
import { useNavigation } from "../../utility/useNavigation";
import { AsyncButton, ErrorCard, Typography } from "../molecules";
import { AsyncSelectChannelTriggerList } from "../molecules/selectors/AsyncSelectChannelTriggerList";

export const EmailConfiguration = ({ channel }) => {
  const token = useAccessToken();
  const { navigate } = useNavigation();
  const [error, setError] = useState();
  const [saving, setSaving] = useState(false);
  const [listTrigger, setListTrigger] = useState({
    listId: "",
    listName: "",
  });

  const handleSave = () => {
    setError(null);
    setSaving(true);

    updateEmailChannelTriggerAsync({
      token,
      id: channel.id,
      listTrigger,
    })
      .then(() => navigate({ pathname: `/channels/detail/${channel.id}` }))
      .catch((e) => setError(e))
      .finally(() => setSaving(false));
  };

  useEffect(() => {
    if (channel.loading) {
      return;
    }
    setListTrigger({
      listId: channel.listTriggerId,
      listName: channel.listTriggerName,
    });
  }, [channel]);

  return (
    <React.Fragment>
      {error ? <ErrorCard error={error} /> : null}
      <div className="mt-3">
        <Typography variant="h4">
          Choose the list that triggers the channel
        </Typography>
        <AsyncSelectChannelTriggerList
          integratedSystemId={channel.linkedIntegratedSystemId}
          value={{ list_id: listTrigger.listId }}
          onChange={(v) =>
            setListTrigger({
              listId: v.value.list_id,
              listName: v.value.list_name,
            })
          }
        />
      </div>

      <AsyncButton
        className="float-right mt-3 btn btn-primary"
        loading={saving}
        disabled={listTrigger.listId == channel.triggerListId}
        onClick={handleSave}
      >
        Save
      </AsyncButton>
    </React.Fragment>
  );
};

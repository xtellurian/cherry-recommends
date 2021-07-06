import React from "react";
import { useRouteMatch, Link } from "react-router-dom";
import {
  ButtonDropdown,
  DropdownMenu,
  DropdownToggle,
  DropdownItem,
} from "reactstrap";
import { useTrackedUser } from "../../api-hooks/trackedUserApi";
import { useUserEvents } from "../../api-hooks/eventApi";
import { BackButton } from "../molecules/BackButton";
import { Subtitle, Title, ErrorCard, Spinner } from "../molecules";
import { JsonView } from "../molecules/JsonView";
import { EventTimelineChart } from "../molecules/EventTimelineChart";

export const TrackedUserDetail = () => {
  const [isOpen, setIsOpen] = React.useState(false);
  const { params } = useRouteMatch();
  const id = params["id"];
  const trackedUser = useTrackedUser({ id });
  const { result } = useUserEvents({ commonUserId: trackedUser?.commonUserId });
  const toggle = () => {
    setIsOpen(!isOpen);
  };
  if (!trackedUser || trackedUser.loading) {
    return (
      <React.Fragment>
        <BackButton className="float-right" to={`/tracked-users`}>
          All Users
        </BackButton>
        <Title>Tracked User</Title>
        <Subtitle>...</Subtitle>
        <hr />
        <Spinner />
      </React.Fragment>
    );
  }
  return (
    <React.Fragment>
      <div className="float-right ml-1">
        <ButtonDropdown isOpen={isOpen} toggle={toggle}>
          <Link to={`/tracked-users/touchpoints/${id}`}>
            <button id="caret" className="btn btn-primary float-right">
              Touchpoints
            </button>
          </Link>
          <DropdownToggle split color="primary" />
          <DropdownMenu>
            <DropdownItem header>More Options</DropdownItem>
            <Link
              to={`/tracked-users/link-to-integrated-system/${trackedUser.id}`}
            >
              <DropdownItem>Link Integrated System</DropdownItem>
            </Link>
          </DropdownMenu>
        </ButtonDropdown>
      </div>

      <BackButton className="float-right" to="/tracked-users">
        All Users
      </BackButton>
      <Title>Tracked User</Title>
      <Subtitle>{trackedUser.name || trackedUser.commonUserId}</Subtitle>
      <hr />
      {trackedUser.error && <ErrorCard error={trackedUser.error} />}
      <JsonView data={trackedUser} />
      <hr />
      <div className="mb-5">
        <h4>Events</h4>
        <EventTimelineChart eventResponse={result} />
      </div>
    </React.Fragment>
  );
};

import React, { Component } from "react";
import { Link } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

const Profile = () => {
  const { user, isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <div>Loading ...</div>;
  }

  return (
    isAuthenticated && (
      <div>
        <img src={user.picture} alt={user.name} />
        <h2>{user.name}</h2>
        <p>{user.email}</p>
      </div>
    )
  );
};

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Hello, world!</h1>
        <p>Welcome to to the prerelease version of Four2 SignalBox</p>

        <p>With SignalBox you can</p>
        <ul>
          <li>
            <strong>Optimise</strong> offers for different customers.
          </li>
          <li>
            <strong>Track</strong> your best performing offers.
          </li>
          <li>
            <strong>Segment</strong> users to enable differential journeys.
          </li>
          <li>
            <strong>Learn</strong> from event data of users.
          </li>
        </ul>

        <div>
          <h3>Demo Apps</h3>

          <Link to="/demo/beer">
            <button>Beer</button>
          </Link>
          <Link to="/demo/shampoo">
            <button>Shampoo</button>
          </Link>
          <Link to="/demo/software">
            <button>Software</button>
          </Link>
        </div>
      </div>
    );
  }
}

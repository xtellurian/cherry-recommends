import React from "react";

export const LearnMoreAboutFeatureGenerators = () => {
  return (
    <div>
      <h4>Feature Generators</h4>
      <p>
        Feature Generators are rules that automatically calculate values of
        Features on every customer tracked in Cherry.
      </p>

      <p>
        For example, if you're creating a Feature for Lifetime Customer Revenue,
        you'll need to tell Cherry how to calculate that value. Cherry can
        calculate Feature values based on all Events collected from a Tracked
        User.
      </p>

      <h5>Filter - Select - Aggregate</h5>
      <p>
        Filter Select Aggregate is the pattern used by Cherry to calculate
        Feature values.
      </p>
      <p>
        The Filter step is optional. You can filter to match only events of
        particular Event Type.
      </p>
      <p>
        The Select step is mandatory. You must select which Event property you
        wish to count (e.g. Value, amount etc.) It is case senstitive.
      </p>
      <p>
        The Aggregate step is optional, but defaults to Sum. This tells Cherry
        whether to average or sum all the values it finds in the Events.
      </p>
    </div>
  );
};

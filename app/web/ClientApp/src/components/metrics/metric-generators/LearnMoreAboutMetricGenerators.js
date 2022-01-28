import React from "react";

const LearnMoreAboutMetricGenerators = () => {
  return (
    <div>
      <h4>Metric Generators</h4>
      <p>
        Metric Generators are rules that automatically calculate values of
        Metrics on every customer tracked in Cherry.
      </p>

      <p>
        For example, if you're creating a Metric for Lifetime Customer Revenue,
        you'll need to tell Cherry how to calculate that value. Cherry can
        calculate Metric values based on all Events collected from a Tracked
        User.
      </p>

      <h5>Filter - Select - Aggregate</h5>
      <p>
        Filter Select Aggregate is the pattern used by Cherry to calculate
        Metric values.
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

export default LearnMoreAboutMetricGenerators;
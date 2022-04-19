import React from "react";
import { Row, Col } from "reactstrap";

import { AdvancedSettings } from "./AdvancedSettings";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { PromotionRecommenderLayout } from "./PromotionRecommenderLayout";
import { RecommenderCard } from "../RecommenderCard";
import {
  ItemRecommenderClone,
  ItemRecommenderDelete,
} from "./ItemsRecommenderDetail";

import "./Advanced.css";

const SettingsNav = ({ nav }) => {
  const scrollTo = ({ element }) => {
    const yOffset = -87;
    const y =
      element.getBoundingClientRect().top + window.pageYOffset + yOffset;

    window.scrollTo({ top: y, behavior: "smooth" });
  };

  return (
    <React.Fragment>
      {nav.map((n) => (
        <Col key={n.id} className="text-center">
          <span
            className="cursor-pointer advanced-nav-item"
            onClick={() => {
              scrollTo({ element: n.ref.current });
            }}
          >
            <img src={n.icon} role="img" alt={n.label} className="mr-2" />
            {n.label}
          </span>
        </Col>
      ))}

      <Col className="text-center">
        <ItemRecommenderClone iconClassName="advanced-nav-item" />
      </Col>

      <Col className="text-center">
        <ItemRecommenderDelete iconClassName="advanced-nav-item" />
      </Col>
    </React.Fragment>
  );
};

export const SettingsPage = () => {
  const learningMetricsRef = React.useRef(null);
  const triggersRef = React.useRef(null);
  const argumentsRef = React.useRef(null);
  const advancedRef = React.useRef(null);

  const advancedMenu = [
    {
      id: "learning-metrics",
      label: "Learning Metrics",
      icon: "/icons/metric-2.svg",
      ref: learningMetricsRef,
    },
    {
      id: "triggers",
      label: "Triggers",
      icon: "/icons/triggers.svg",
      ref: triggersRef,
    },
    {
      id: "arguments",
      label: "Arguments",
      icon: "/icons/argument.svg",
      ref: argumentsRef,
    },
    {
      id: "advanced",
      label: "Advanced",
      icon: "/icons/settings.svg",
      ref: advancedRef,
    },
  ];

  return (
    <PromotionRecommenderLayout>
      <Row className="mt-4">
        <SettingsNav nav={advancedMenu} />
      </Row>

      <RecommenderCard ref={learningMetricsRef}>
        <LearningMetrics />
      </RecommenderCard>

      <RecommenderCard ref={triggersRef} title="Triggers">
        <Triggers />
      </RecommenderCard>

      <RecommenderCard ref={argumentsRef} title="Arguments">
        <Arguments />
      </RecommenderCard>

      <RecommenderCard ref={advancedRef} title="Advanced">
        <AdvancedSettings />
      </RecommenderCard>
    </PromotionRecommenderLayout>
  );
};

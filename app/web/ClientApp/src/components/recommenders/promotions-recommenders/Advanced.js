import React from "react";
import { Row, Col } from "reactstrap";

import { Settings } from "./Settings";
import { Destinations } from "./Destinations";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { ItemRecommenderLayout } from "./ItemRecommenderLayout";
import { RecommenderCard } from "../RecommenderCard";
import {
  ItemRecommenderClone,
  ItemRecommenderDelete,
} from "./ItemsRecommenderDetail";

import "./Advanced.css";

const AdvancedNav = ({ nav }) => {
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

export const Advanced = () => {
  const targetMetricRef = React.useRef(null);
  const triggersRef = React.useRef(null);
  const argumentsRef = React.useRef(null);
  const integrationsRef = React.useRef(null);
  const toolsRef = React.useRef(null);

  const advancedMenu = [
    {
      id: "target-metric",
      label: "Target Metrics",
      icon: "/icons/metric-2.svg",
      ref: targetMetricRef,
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
      id: "integrations",
      label: "Integrations",
      icon: "/icons/integration.svg",
      ref: integrationsRef,
    },
    {
      id: "tools",
      label: "Tools",
      icon: "/icons/settings.svg",
      ref: toolsRef,
    },
  ];

  return (
    <ItemRecommenderLayout>
      <Row className="mt-4">
        <AdvancedNav nav={advancedMenu} />
      </Row>

      <RecommenderCard ref={targetMetricRef}>
        <LearningMetrics />
      </RecommenderCard>

      <RecommenderCard ref={triggersRef} title="Triggers">
        <Triggers />
      </RecommenderCard>

      <RecommenderCard ref={argumentsRef} title="Arguments">
        <Arguments />
      </RecommenderCard>

      <RecommenderCard ref={integrationsRef} title="Destinations">
        <Destinations />
      </RecommenderCard>

      <RecommenderCard ref={toolsRef} title="Advanced">
        <Settings />
      </RecommenderCard>
    </ItemRecommenderLayout>
  );
};

import React from "react";
import { Row, Col } from "reactstrap";

import { AdvancedSettings } from "./AdvancedSettings";
import { Triggers } from "./Triggers";
import { LearningMetrics } from "./LearningMetrics";
import { Arguments } from "./Arguments";
import { PromotionCampaignLayout } from "./PromotionCampaignLayout";
import { CampaignCard } from "../CampaignCard";
import {
  PromotionCampaignClone,
  PromotionCampaignDelete,
} from "./PromotionsCampaignDetail";

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
        <PromotionCampaignClone iconClassName="advanced-nav-item" />
      </Col>

      <Col className="text-center">
        <PromotionCampaignDelete iconClassName="advanced-nav-item" />
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
    <PromotionCampaignLayout>
      <Row className="mt-4">
        <SettingsNav nav={advancedMenu} />
      </Row>

      <CampaignCard ref={triggersRef} title="Triggers">
        <Triggers />
      </CampaignCard>

      <CampaignCard ref={argumentsRef} title="Arguments">
        <Arguments />
      </CampaignCard>

      <CampaignCard ref={learningMetricsRef} title="Learning Metrics">
        <LearningMetrics />
      </CampaignCard>

      <CampaignCard ref={advancedRef} title="Advanced">
        <AdvancedSettings />
      </CampaignCard>
    </PromotionCampaignLayout>
  );
};

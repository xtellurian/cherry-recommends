import React from "react";

import { useInterval } from "../../utility/useInterval";

import "./PersonalizationCarousel.css";

const items = [
  {
    image: "/images/personalization_1.png",
    header: "What you offer doesn't make all people happy",
    description:
      "A single offer will make some customers very happy, some moderately and some indifferent or unhappy.",
  },
  {
    image: "/images/personalization_2.png",
    header: "A/B testing can make a marginal improvement",
    description:
      "A/B testing will help increase the optimised metric, e.g. happiness. But you still only have a single offer. You will still have a mix of happy, indifferently, and unhappy customers.",
  },
  {
    image: "/images/personalization_3.png",
    header: "Personalization is the best",
    description:
      "Personalization delivers multiple offers increasing the happiness of your customers. Customers are far more satisfied by more relevant and tailored offers.",
  },
];

export const PersonalizationCarousel = () => {
  const [activeIndex, setActiveIndex] = React.useState(0);

  const handleNext = () => {
    setActiveIndex((oldActiveIndex) => {
      if (oldActiveIndex === items.length - 1) {
        return 0;
      }

      return oldActiveIndex + 1;
    });
  };

  useInterval(() => handleNext(), 20000);

  return (
    <div className="carousel slide" data-ride="carousel">
      <ol className="carousel-indicators">
        {items.map((item, index) => (
          <li
            key={item.header}
            className={`bg-secondary circle ${
              index === activeIndex ? "active" : ""
            }`}
            onClick={() => setActiveIndex(index)}
          ></li>
        ))}
      </ol>
      <div className="carousel-inner">
        {items.map((item, index) => (
          <div
            key={item.header}
            className={`carousel-item ${index === activeIndex ? "active" : ""}`}
          >
            <div className="mx-5 mt-4 mb-5">
              <h5 className="text-center">{item.header}</h5>
              <img
                className="d-block w-75 mx-auto"
                src={item.image}
                alt={`Slide ${index + 1}`}
              />
              <p className="text-center">{item.description}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

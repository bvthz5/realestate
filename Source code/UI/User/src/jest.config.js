module.exports = {
  clearMocks: false,
  collectCoverageFrom: ["src/**/*.{js,jsx,mjs}"],
  coverageReporters: ["html", "text", "text-summary", "cobertura"],
  coverageDirectory: "coverage",
  moduleFileExtensions: ["js", "json", "jsx", "module.css", "css"],
  testEnvironment: "jsdom",
  testMatch: ["**/__tests__/**/*.js?(x)", "**/?(*.)+(spec|test).js?(x)"],
  testPathIgnorePatterns: ["\\\\node_modules\\\\"],
  testURL: "http://localhost",
  transformIgnorePatterns: ["<rootDir>/node_modules/"],
  verbose: false,
  moduleNameMapper: {
    "\\.(jpg|jpeg|png|gif)$": "identity-obj-proxy",
    "^axios$": require.resolve("axios"),
  },
  transform: {
    "^.+\\.js$": "babel-jest",
    "^.+\\.jsx$": "babel-jest",
    ".+\\.(css|styl|less|sass|scss)$":
      "<rootDir>/node_modules/jest-css-modules-transform",
  },
};

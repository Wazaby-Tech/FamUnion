module.exports = {
  presets: ['module:metro-react-native-babel-preset'],
  // Skip the dotenv plugin in the test environment so jest's moduleNameMapper
  // can intercept `import { APIURL } from '@env'` via __mocks__/@env.js.
  plugins: process.env.NODE_ENV !== 'test' ? [['module:react-native-dotenv']] : [],
};

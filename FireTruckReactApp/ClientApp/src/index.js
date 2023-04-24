import {createRoot} from 'react-dom/client';
import App from './App';
import Bugsnag from '@bugsnag/js'
import React from 'react'
import BugsnagPluginReact from '@bugsnag/plugin-react'

console.info(process.env.REACT_APP_BUGSNAG_API_KEY);

Bugsnag.start({
    apiKey: process.env.REACT_APP_BUGSNAG_API_KEY,
    plugins: [new BugsnagPluginReact()],
    appVersion: '0.1.0',
    sendCode: true
});

const ErrorBoundary = Bugsnag.getPlugin('react')
    .createErrorBoundary(React)
const ErrorView = () =>
    <div>
        <p>Inform users of an error in the component tree.</p>
    </div>

createRoot(document.getElementById('root')).render(
    <ErrorBoundary FallbackComponent={ErrorView}>
        <App/>
    </ErrorBoundary>);

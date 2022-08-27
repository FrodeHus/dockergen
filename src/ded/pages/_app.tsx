import "../styles/globals.css";
import type { AppProps } from "next/app";
import { MsalProvider } from "@azure/msal-react";
import { MsalInstance } from "../modules/auth/keycloak";

function MyApp({ Component, pageProps }: AppProps) {
  <MsalProvider instance={MsalInstance}>
    return <Component {...pageProps} />
  </MsalProvider>;
}

export default MyApp;

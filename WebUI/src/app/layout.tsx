import { Nunito } from "next/font/google";
import "./globals.css";
import { AosInit } from "./_components/aos-init";
import { Header } from "@/components/ui/header";


const nunito = Nunito({
  subsets: ["latin"],
  weight: ["400", "600", "700"],
  variable: "--font-nunito",
  display: "swap",
});


export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="pt-BR" className="">
      <body
        className={`antialiased`} suppressHydrationWarning>
        <Header/>
        {children}
        <AosInit />
      </body>
    </html>
  );
}

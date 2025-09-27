import { ReactNode } from "react";
import Navbar from "../Components/Navbar";
import Header from "../Components/Header";

interface LayoutProps {
  children: ReactNode;
}

export default function Layout({ children }: LayoutProps) {
  return (
    <div className="flex h-screen bg-gray-100">
      <div className="w-64 bg-white shadow-md">
        <Navbar />
      </div>

      <div className="flex-1 flex flex-col">
        <div className="bg-white shadow-sm p-4">
          <Header />
        </div>

        <div className="flex-1 p-6 overflow-auto">{children}</div>
      </div>
    </div>
  );
}

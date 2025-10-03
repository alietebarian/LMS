import { ReactNode, useEffect } from "react";
import Navbar from "../Components/Navbar";
import Header from "../Components/Header";
import { useNavigate } from "react-router-dom";

interface LayoutProps {
  children: ReactNode;
}

export default function Layout({ children }: LayoutProps) {

  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem("token");

    if (!token) navigate("/login");
  }, [navigate])

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

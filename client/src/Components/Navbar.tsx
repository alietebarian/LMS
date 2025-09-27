import { ReactNode } from "react";
import { GoHome } from "react-icons/go";
import { IoMdBook } from "react-icons/io";
import { PiStudentDuotone } from "react-icons/pi";
import { RiShutDownLine } from "react-icons/ri";
import { NavLink } from "react-router-dom";

interface menuItemInterface {
  id: number;
  icon: ReactNode;
  title: string;
  path: string
}

export default function Navbar() {
  const navMenu: menuItemInterface[] = [
    { id: 1, icon: <GoHome />, title: "Home", path: '/' },
    { id: 2, icon: <PiStudentDuotone />, title: "Courses", path: '/courses' },
    { id: 3, icon: <IoMdBook />, title: "Lessons", path: '/lessons' },
  ];

  return (
    <aside className="bg-white h-screen w-64 shadow-md flex flex-col">
      <div className="flex items-center justify-center gap-2 px-6 py-4 mt-3">
        <RiShutDownLine className="text-4xl text-blue-600 cursor-pointer" />
        <span className="font-bold text-2xl">SkillUp</span>
      </div>
      <ul className="flex-1 mt-6 space-y-2 px-4">
        {navMenu.map((item) => (
          <NavLink
            key={item.id}
            to={item.path.toLowerCase()}
            className={({ isActive }) =>
              `flex items-center gap-3 px-3 py-2 rounded-md cursor-pointer hover:bg-gray-100 transition-colors ${
                isActive ? "bg-gray-100 transition-colors" : ""
              }`
            }
          >
            <span className="text-xl text-gray-600">{item.icon}</span>
            <span className="text-gray-700 font-medium">{item.title}</span>
          </NavLink>
        ))}
      </ul>
    </aside>
  );
}

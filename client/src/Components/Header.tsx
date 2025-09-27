import { IoSearchSharp } from "react-icons/io5";
import { NavLink } from "react-router-dom";

export default function Header() {
  const HeaderItems: string[] = ["Courses", "Blog", "Music"];

  return (
    <header className="w-full px-8 py-2 flex items-center justify-between">
      <ul className="flex space-x-8 text-gray-700 font-medium">
        {HeaderItems.map((item) => (
          <NavLink
            key={item}
            to={`/${item.toLowerCase()}`}
            className={({ isActive }) =>
              `cursor-pointer hover:text-black transition-colors ${
                isActive ? "text-black font-bold" : ""
              }`
            }
          >
            {item}
          </NavLink>
        ))}
      </ul>

      <div className="flex px-3">
        <div className="flex items-center border rounded-md px-3 py-1 bg-gray-50 mx-3">
          <IoSearchSharp className="text-gray-500 text-lg mr-2 cursor-pointer" />
          <input
            type="text"
            placeholder="Search..."
            className="outline-none bg-transparent text-sm"
          />
        </div>
        <button className="cursor-pointer bg-black text-white rounded-md py-2 px-6 font-semibold hover:bg-gray-800 transition-colors">
          Sign In
        </button>
      </div>
    </header>
  );
}

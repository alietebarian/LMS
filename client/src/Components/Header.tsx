import { useEffect, useState } from "react";
import { IoSearchSharp } from "react-icons/io5";
import { Link, NavLink } from "react-router-dom";

interface userInterface{
  fullName: string
  id: string
  email: string
  role: string
}

export default function Header() {
  const HeaderItems: string[] = ["Courses", "Blog", "Contactus"];

  const [ token, setToken ] = useState<string | null>(null)
  useEffect(() => {
    setToken(localStorage.getItem('token'))
  },[])

  const deleteToken = () => {
    localStorage.removeItem('token')
    setToken(null)
  }
  
  const userToken = localStorage.getItem('user')
  let parseToken : userInterface | null = null

  if(userToken){
    parseToken = JSON.parse(userToken) as userInterface
  }
  

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
        {!token ? (
          <>
            <Link to={"/register"}>
              <button className="cursor-pointer bg-gray-800 text-white rounded-md py-2 px-6 font-semibold hover:bg-gray-800 transition-colors">
                Register
              </button>
            </Link>
            <Link to={"/login"}>
              <button className="cursor-pointer bg-blue-800 text-white rounded-md mx-3 py-2 px-6 font-semibold hover:bg-blue-900 transition-colors">
                Log In
              </button>
            </Link>
          </>
        ) : (
          <>
            <button
              onClick={deleteToken}
              className="cursor-pointer bg-red-600 text-white rounded-md mx-3 py-2 px-6 font-semibold hover:bg-red-900 transition-colors"
            >
              Log Out
            </button>
            <span className="m-auto font-semibold rounded-md">
              welcome {parseToken?.fullName}
            </span>
          </>
        )}
      </div>
    </header>
  );
}

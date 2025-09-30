import { useMutation } from "@tanstack/react-query"
import React, { useState } from "react"
import { useNavigate } from "react-router-dom"
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import Swal from "sweetalert2";

interface userInput{
    userName: string
    password: string
}

interface DecodeToken{
    fullName: string
    id: string
    email: string
    role: string
}

export default function Login() {

    const [ userInput, setUserInput ] = useState<userInput>({
        userName: "",
        password: ""
    })

    const [ message, setMessage ] = useState<string | null>(null)
    const navigate = useNavigate()

    const handleUserInput = (e : React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        setUserInput(prev => ({...prev, [name]: value}))
    }

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setMessage(null)
        loginMutation.mutate()
    }

    const loginMutation = useMutation({
      mutationFn: async () => {
        const res = await axios.post("http://localhost:5000/api/Auth/login", {
          userName: userInput.userName,
          password: userInput.password,
        });

        const token = res.data.token;
        localStorage.setItem("token", token);

        const { fullName, id, email, role } = jwtDecode<DecodeToken>(
          res.data.token
        );

        localStorage.setItem(
          "user",
          JSON.stringify({ fullName, id, email, role })
        );
      },      

      onSuccess: () => {
        Swal.fire({
          title: "Good job!",
          text: "ورود با موفقیت انجام شد",
          icon: "success",
        });
        navigate("/");
      },
      onError: (error: any) => {
        const errMsg = error?.response?.data;
        setMessage(typeof errMsg === "string" ? errMsg : "خطا در ورود");
        Swal.fire({
          title: "Oops!",
          text: "Username or password is incorrect !",
          icon: "error",
        });
      },
    });
    console.log(localStorage.getItem('user'));
    
  return (
    <div className="max-w-md mx-auto mt-16 p-6 bg-white shadow-md rounded-lg">
      <form className="space-y-6" onSubmit={handleSubmit}>
        <h1 className="text-2xl font-bold text-center">ورود</h1>

        {message && (
          <div className="bg-yellow-100 text-yellow-800 px-4 py-2 rounded">
            {message}
          </div>
        )}

        <input
          type="text"
          placeholder="نام کاربری"
          name="userName"
          required
          onChange={handleUserInput}
          className="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
        />

        <input
          type="password"
          placeholder="رمز عبور"
          name="password"
          required
          onChange={handleUserInput}
          className="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
        />

        <div className="text-center">
          <button
            type="submit"
            className="w-48 bg-blue-600 text-white px-4 py-2 rounded cursor-pointer hover:bg-blue-700 transition"
          >
            ورود
          </button>
        </div>
      </form>
    </div>
  );
}

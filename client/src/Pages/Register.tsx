import { useState } from 'react';
import { SD_Role } from '../Utility/SD';
import axios from 'axios';
import { useMutation } from '@tanstack/react-query';

export default function Register() {

    const [userInput, setUserInput] = useState({
      name: "",
      userName: "",
      password: "",
      role: "",
    });

    const [message, setMessage] = useState<string | null>(null);

    const registerMutation = useMutation({
      mutationFn: async () => {
        const res = await axios.post(
          "http://localhost:5000/api/Auth/register",
          {
            name: userInput.name,
            userName: userInput.userName,
            password: userInput.password,
            role: userInput.role,
          }
        );
        return res.data;
      },
      onSuccess: (data: any) => {
        setMessage("ثبت‌نام با موفقیت انجام شد");
        localStorage.setItem('userRole', data.role)
      },
      onError: (error: any) => {
        const errMsg = error?.response?.data;
        setMessage(typeof errMsg === "string" ? errMsg : "خطا در ثبت‌نام");
      },
    });

    const handleUserInput = (
      e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
    ) => {
      const { name, value } = e.target;
      setUserInput((prev) => ({
        ...prev,
        [name]: value,
      }));
    };

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
      e.preventDefault();
      setMessage(null);
      registerMutation.mutate();
    };
    

  return (
    <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow-md rounded-lg">
      <form method="post" className="space-y-6" onSubmit={handleSubmit}>
        <h1 className="text-2xl font-bold text-center">ثبت‌نام</h1>

        {message && (
          <div className="bg-yellow-100 text-yellow-800 px-4 py-2 rounded">
            {message}
          </div>
        )}

        <input
          type="text"
          className="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="نام کاربری"
          required
          name="userName"
          value={userInput.userName}
          onChange={handleUserInput}
        />

        <input
          type="text"
          className="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="نام کامل"
          required
          name="name"
          value={userInput.name}
          onChange={handleUserInput}
        />

        <input
          type="password"
          className="w-full px-4 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500"
          placeholder="رمز عبور"
          required
          name="password"
          value={userInput.password}
          onChange={handleUserInput}
        />

        <select
          className="w-full px-4 py-2 border border-gray-300 rounded bg-white focus:outline-none focus:ring-2 focus:ring-blue-500"
          required
          name="role"
          value={userInput.role}
          onChange={handleUserInput}
        >
          <option value="">-- انتخاب نقش --</option>
          <option value={`${SD_Role.Admin}`}>Admin</option>
          <option value={`${SD_Role.Instructor}`}>Instructor</option>
          <option value={`${SD_Role.Student}`}>Student</option>
        </select>

        <div className="text-center">
          <button
            type="submit"
            className="bg-green-600 text-white px-6 py-2 rounded hover:bg-green-700 transition"
          >
            ثبت‌نام
          </button>
        </div>
      </form>
    </div>
  );
}

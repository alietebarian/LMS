import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

export default function Home() {

  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem('token')

    if(!token)
      navigate('/login')
  },[navigate])

  return (
        <main className="p-6">
          <h1 className="text-2xl font-bold mb-4">Welcome to SkillUp LMS 🎓</h1>
          <p className="text-gray-600">
            اینجا می‌تونی لیست دوره‌ها، درس‌ها و داشبورد خودت رو ببینی.
          </p>
        </main>
  );
}

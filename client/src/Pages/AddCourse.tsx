import { useMutation, useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

interface AddCourseInterface {
  title: string;
  instructorId: string;
}

const addCourseRequest = async (
  course: AddCourseInterface
): Promise<AddCourseInterface> => {
  const res = await axios.post("http://localhost:5000/api/Course", course);
  return res.data;
};

export default function AddCourse() {
  const [title, setTitle] = useState("");
  const navigate = useNavigate()

  const userToken = localStorage.getItem("user");
  let parseToken = null;
  if (userToken) {
    parseToken = JSON.parse(userToken);
  }

  const loggedInUserId = parseToken?.id; // logged in user id

  const queryClient = useQueryClient();
  const { mutate: addCourse, isPending } = useMutation({
    mutationFn: addCourseRequest,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["courses"] });
      setTitle(""); // reset input
    },
    onError: (error: any) => {
      alert("خطا در افزودن دوره: " + error.message);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    if (!loggedInUserId) {
      alert("کاربر وارد نشده است!");
      return;
    }

    addCourse({ title, instructorId: loggedInUserId });
    navigate('/courses')
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded-xl shadow-md w-full max-w-md space-y-6"
      >
        <h1 className="text-2xl font-bold text-center text-gray-800">
          Add Course
        </h1>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Title
          </label>
          <input
            type="text"
            placeholder="Enter course title..."
            name="title"
            required
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="w-full border border-gray-300 rounded-md px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div className="text-center">
          <button
            type="submit"
            disabled={isPending}
            className="w-full bg-blue-600 text-white py-2 rounded-md font-semibold hover:bg-blue-700 transition-colors disabled:opacity-50"
          >
            {isPending ? "Adding..." : "Add Course"}
          </button>
        </div>
      </form>
    </div>
  );
}

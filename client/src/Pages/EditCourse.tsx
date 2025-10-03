import { useMutation, useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";

interface EditCourseInterface {
  title: string;
}

const editCourse = async (id: number, data: EditCourseInterface) => {
  const res = await axios.put(`http://localhost:5000/api/Course/${id}`, data);
  return res.data;
};

export default function EditCourse() {
  const { id } = useParams();
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const [title, setTitle] = useState<string>("");

  const { mutate, isLoading } = useMutation({
    mutationFn: (newData: EditCourseInterface) =>
      editCourse(Number(id), newData),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["courses"] });
      navigate("/courses");
    },
    onError: (error: any) => {
      alert("خطا در ویرایش دوره: " + error.message);
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) {
      alert("عنوان دوره نمی‌تواند خالی باشد");
      return;
    }
    mutate({ title });
  };

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 px-4">
      <form
        onSubmit={handleSubmit}
        className="bg-white p-8 rounded-xl shadow-md w-full max-w-md space-y-6"
      >
        <h1 className="text-2xl font-bold text-center text-gray-800">
          Edit Course
        </h1>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-2">
            Title
          </label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="Enter course title..."
            required
            className="w-full border border-gray-300 rounded-md px-4 py-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div className="text-center">
          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-blue-600 text-white py-2 rounded-md font-semibold hover:bg-blue-700 transition-colors disabled:opacity-50"
          >
            {isLoading ? "Updating..." : "Update Course"}
          </button>
        </div>
      </form>
    </div>
  );
}

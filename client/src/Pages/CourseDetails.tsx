import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { useParams } from "react-router-dom";

interface CourseInterface {
  id: number;
  title: string;
  instructorName: string;
}

const fetchCourse = async (id: number): Promise<CourseInterface> => {
  const res = await axios.get(`http://localhost:5000/api/Course/${id}`);
  return res.data;
};

export default function CourseDetails() {
  const { id } = useParams();

  const {
    data: course,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["courses", id],
    queryFn: () => fetchCourse(Number(id)),
    enabled: !!id,
  });

  if (isLoading)
    return (
      <div className="flex justify-center items-center min-h-screen text-lg font-semibold text-gray-600">
        Loading...
      </div>
    );

  if (isError)
    return (
      <div className="flex justify-center items-center min-h-screen text-lg font-semibold text-red-500">
        Error loading course details.
      </div>
    );

  return (
    <div className="flex justify-center items-center min-h-screen bg-gray-100 px-4">
      <div className="bg-white rounded-xl shadow-lg p-8 w-full max-w-md">
        <h1 className="text-2xl font-bold text-gray-800 mb-6 text-center">
          Course Details
        </h1>

        <div className="space-y-4">
          <div>
            <span className="block text-sm font-medium text-gray-500">
              Title
            </span>
            <p className="text-lg font-semibold text-gray-800">
              {course?.title}
            </p>
          </div>

          <div>
            <span className="block text-sm font-medium text-gray-500">
              Instructor
            </span>
            <p className="text-lg text-gray-700">{course?.instructorName}</p>
          </div>
        </div>

        <div className="mt-6 flex justify-center">
          <button className="px-6 py-2 cursor-pointer bg-blue-600 text-white font-semibold rounded-lg shadow hover:bg-blue-700 transition">
            Enroll Now
          </button>
        </div>
      </div>
    </div>
  );
}

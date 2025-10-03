import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import axios from "axios";
import { BsEye } from "react-icons/bs";
import { IoTrashOutline } from "react-icons/io5";
import { TiPencil } from "react-icons/ti";
import { Link } from "react-router-dom";

interface UserInterface {
  fullName: string;
  id: string;
  email: string;
  role: string;
}

interface CoursesInterface {
  id: number;
  title: string;
  instructorName: string;
  structorId?: string; // اگه از API میاد
}

const fetchCourses = async (): Promise<CoursesInterface[]> => {
  const res = await axios.get("http://localhost:5000/api/Course");
  return res.data;
};

const deleteCourses = async (id: number) => {
  const res = await axios.delete(`http://localhost:5000/api/Course/${id}`);
  return res.data;
};

export default function Courses() {
  const {
    data: courses,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["courses"],
    queryFn: fetchCourses,
  });

  const queryClient = useQueryClient();
  const { mutate: deleteCourse } = useMutation({
    mutationFn: deleteCourses,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["courses"] });
    },
    onError: (error: any) => {
      alert("خطا در حذف دوره: " + error.message);
    },
  });

  if (isLoading) return <div>Loading...</div>;
  if (isError) return <div>Error...</div>;

  const userToken = localStorage.getItem("user");
  let parseToken: UserInterface | null = null;
  if (userToken) {
    parseToken = JSON.parse(userToken) as UserInterface;
  }

  const currentCourses = courses?.filter(
    (course) => course.structorId === parseToken?.id
  );

  return (
    <div className="p-6">
      <div className="flex justify-between text-center">
        <h1 className="text-2xl font-bold mb-4">All Courses</h1>
        <Link to={"/addcourse"}>
          <button className="cursor-pointer bg-gray-500 text-white rounded-md mb-3 py-2 px-6 font-semibold hover:bg-gray-700 transition-colors">
            Add Course
          </button>
        </Link>
      </div>

      <div className="overflow-x-auto shadow-lg rounded-lg border border-gray-200">
        {currentCourses && currentCourses.length > 0 ? (
          <table className="min-w-full bg-white text-sm text-left">
            <thead className="bg-gray-100 text-gray-700 uppercase text-xs">
              <tr>
                <th className="px-6 py-3 border-b">Instructor Name</th>
                <th className="px-6 py-3 border-b">Title</th>
                <th className="px-6 py-3 border-b text-center">Operation</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-200">
              {currentCourses.map((course, index) => (
                <tr
                  className={`hover:bg-gray-50 ${
                    index % 2 === 0 ? "bg-white" : "bg-gray-50/50"
                  }`}
                  key={course.id}
                >
                  <td className="px-6 py-4 font-medium text-gray-900">
                    {course?.instructorName}
                  </td>
                  <td className="px-6 py-4 text-gray-700">{course?.title}</td>
                  <td className="px-6 py-4">
                    <div className="flex items-center justify-center gap-4">
                      <IoTrashOutline
                        title="delete"
                        className="cursor-pointer text-xl text-red-500 hover:text-red-700 transition"
                        onClick={() => deleteCourse(course.id)}
                      />
                      <Link to={`/editcourse/${course.id}`}>
                        <TiPencil
                          title="edit"
                          className="cursor-pointer text-xl text-blue-500 hover:text-blue-700 transition"
                        />
                      </Link>
                      <Link to={`/courses/${course.id}`}>
                        <BsEye
                          title="view"
                          className="cursor-pointer text-xl text-green-500 hover:text-green-700 transition"
                        />
                      </Link>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <div className="font-bold text-2xl text-center text-red-500 p-15">
            There is nothing to show
          </div>
        )}
      </div>
    </div>
  );
}

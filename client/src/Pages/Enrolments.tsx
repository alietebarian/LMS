import { useQuery } from "@tanstack/react-query";
import axios from "axios";
import { BsEye } from "react-icons/bs";
import { Link } from "react-router-dom";

interface CourseInterface {
  id: number;
  title: string;
  instructorName?: string | null;
  structorId?: string | null;
}

interface InstructorInterface {
  id: string;
  name: string;
  userName: string;
  coursesTaught: CourseInterface[];
}

interface UserInterface {
  fullName: string;
  id: string;
  email: string;
  role: string;
}

const getInstructorCourses = async (): Promise<InstructorInterface[]> => {
  const res = await axios.get("http://localhost:5000/api/Instructor");
  return res.data;
};

export default function Enrolments() {
  const userToken = localStorage.getItem("user");
  let parseToken: UserInterface | null = null;

  if (userToken) {
    parseToken = JSON.parse(userToken) as UserInterface;
  }

  const {
    data: instructors,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["instructor"],
    queryFn: getInstructorCourses,
  });

  if (isLoading) return <div className="p-4">Loading...</div>;
  if (isError)
    return <div className="p-4 text-red-500">Error fetching data</div>;

  const currentInstructor = instructors?.find(
    (inst) => inst.id === parseToken?.id
  );

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-6">Hello {parseToken?.fullName}</h1>

      {!currentInstructor ? (
        <p className="text-gray-600">
          شما به عنوان Instructor ثبت نشده‌اید یا هیچ دوره‌ای ندارید.
        </p>
      ) : (
        <div>
          <h2 className="text-xl font-semibold mb-4">
            دوره‌های {currentInstructor.name}:
          </h2>

          <table className="w-full border border-gray-300 rounded-lg overflow-hidden shadow-md">
            <thead className="bg-gray-200">
              <tr>
                <th className="p-3 text-left">ID</th>
                <th className="p-3 text-left">Title</th>
                  <th className="p-3 text-left items-center justify-center gap-4 flex">
                    Show Course
                  </th>
              </tr>
            </thead>
            <tbody>
              {currentInstructor.coursesTaught.map((course) => (
                <tr
                  key={course.id}
                  className="border-t hover:bg-gray-50 transition"
                >
                  <td className="p-3">{course.id}</td>
                  <td className="p-3">{course.title}</td>
                  <td className="p-3">
                    <div className="items-center justify-center gap-4 flex">
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

              {currentInstructor.coursesTaught.length === 0 && (
                <tr>
                  <td colSpan={3} className="p-3 text-center text-gray-500">
                    هیچ دوره‌ای ثبت نشده است.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}

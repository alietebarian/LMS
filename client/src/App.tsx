import { BrowserRouter, Route, Routes } from 'react-router-dom'
import Home from './Pages/Home'
import NotFound from './Pages/NotFound';
import Base from '../src/layout/Base'
import Login from './Pages/Login';
import Register from './Pages/Register';
import Courses from './Pages/Courses';
import AddCourse from './Pages/AddCourse';
import CourseDetails from './Pages/CourseDetails';
import EditCourse from './Pages/EditCourse';
import Enrolments from './Pages/Enrolments';

export default function App() {
  return (
    <BrowserRouter>
      <Base>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="/courses" element={<Courses />} />
          <Route path="/courses/:id" element={<CourseDetails />} />
          <Route path="/addcourse" element={<AddCourse />} />
          <Route path="/editcourse/:id" element={<EditCourse />} />
          <Route path="/enrolment" element={<Enrolments/>}/>
          <Route path="*" element={<NotFound />} />
        </Routes>
      </Base>
    </BrowserRouter>
  );
}

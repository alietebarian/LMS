import { BrowserRouter, Route, Routes } from 'react-router-dom'
import Home from './Pages/Home'
import NotFound from './Pages/NotFound';
import Base from '../src/layout/Base'
import Login from './Pages/Login';
import Register from './Pages/Register';

export default function App() {
  return (
    <BrowserRouter>
      <Base>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
          <Route path="*" element={<NotFound />} />
        </Routes>
      </Base>
    </BrowserRouter>
  );
}

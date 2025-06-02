import { AuthProvider } from "./Pages/Context/AuthContext";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import  Home  from "./Pages/Home";
import Register from "./Pages/Regsiter";
import User from "./Pages/User/User";

function App() {

  return (
    <>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path ="/" element={<Home></Home>}></Route>
            <Route path ="/register" element={<Register></Register>}></Route>
            <Route path ="/profile" element={<User></User>}></Route>
          </Routes>
        </Router>
      </AuthProvider>
    </>
  )
}

export default App

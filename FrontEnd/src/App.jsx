import { AuthProvider } from "./Pages/Context/AuthContext";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import  Home  from "./Pages/Home";
import Register from "./Pages/Regsiter";

function App() {

  return (
    <>
      <AuthProvider>
        <Router>
          <Routes>
            <Route path ="/" element={<Home></Home>}></Route>
            <Route path ="/register" element={<Register></Register>}></Route>
          </Routes>
        </Router>
      </AuthProvider>
    </>
  )
}

export default App

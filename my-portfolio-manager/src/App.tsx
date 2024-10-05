//import React from 'react';
import { BrowserRouter as Router, Route, Routes, Navigate } from 'react-router-dom';
import Login from './components/Login';
import Register from './components/Register';
import ProjectsList from './components/ProjectsList';
import NewProject from './components/NewProject';
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
    const isAuthenticated = !!localStorage.getItem('authToken');

    return (
        <Router>
            <Routes>
                {/* Public Routes */}
                <Route path="/login" element={<Login />} />
                <Route path="/register" element={<Register />} />

                {/* Protected Routes */}
                {isAuthenticated ? (
                    <>
                        <Route path="/projects" element={<ProjectsList />} />
                        <Route path="/new-project" element={<NewProject />} />
                        <Route path="/" element={<Navigate to="/projects" />} /> {/* Redirect to projects list */}
                    </>
                ) : (
                    <Route path="/" element={<Navigate to="/login" />} /> 
                )}

                {/* Fallback Route */}
                <Route path="*" element={<Navigate to="/login" />} /> {/* Redirect to login for any unknown route */}
            </Routes>
        </Router>
    );
};

export default App;

import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import '../App.css';

export default function Register() {
  const [lang, setLang] = useState(() => {
    return localStorage.getItem('fintracker_lang') || 'fa';
  });

  const changeLang = (newLang) => {
    setLang(newLang);
    localStorage.setItem('fintracker_lang', newLang);
  };

  const t = {
    fa: {
      brand: "FinTracker",
      subtitle: "مدیریت هوشمند مالی • آینده‌ای مطمئن",
      title: "ساخت حساب کاربری",
      desc: "برای شروع مدیریت دارایی‌هایتان ثبت‌نام کنید",
      firstName: "نام",
      lastName: "نام خانوادگی",
      email: "ایمیل",
      phone: "شماره تماس",
      password: "رمزعبور",
      submit: "ثبت‌نام",
      hasAccount: "قبلاً حساب کاربری ساخته‌اید؟",
      loginLink: "وارد شوید"
    },
    en: {
      brand: "FinTracker",
      subtitle: "Smart Financial Management • Secure Future",
      title: "Create Account",
      desc: "Sign up for better financial management",
      firstName: "First Name",
      lastName: "Last Name",
      email: "Email",
      phone: "Phone Number",
      password: "Password",
      submit: "Register",
      hasAccount: "Already have an account?",
      loginLink: "Log in"
    }
  };

  const current = t[lang];

  return (
    <div className="auth-container" dir={lang === 'fa' ? 'rtl' : 'ltr'}>
      <div className="glass-card">
        <div className="lang-switch">
          <button 
            type="button" 
            className={`lang-btn ${lang === 'fa' ? 'active' : ''}`} 
            onClick={() => changeLang('fa')}
          >
            فارسی
          </button>
          <span>|</span>
          <button 
            type="button" 
            className={`lang-btn ${lang === 'en' ? 'active' : ''}`} 
            onClick={() => changeLang('en')}
          >
            English
          </button>
        </div>

        <h1 className="brand-title">{current.brand}</h1>
        <p className="brand-subtitle">{current.subtitle}</p>

        <h2 className="form-title">{current.title}</h2>
        <p className="form-desc">{current.desc}</p>

        <form onSubmit={(e) => e.preventDefault()}>
          <div className="form-row">
            <div className="form-group">
              <label>{current.firstName}</label>
              <input type="text" placeholder={current.firstName} />
            </div>
            <div className="form-group">
              <label>{current.lastName}</label>
              <input type="text" placeholder={current.lastName} />
            </div>
          </div>

          <div className="form-group">
            <label>{current.email}</label>
            <input type="email" placeholder="example@gmail.com" />
          </div>

          <div className="form-group">
            <label>{current.phone}</label>
            <input type="text" placeholder="0912..." />
          </div>

          <div className="form-group">
            <label>{current.password}</label>
            <input type="password" placeholder="••••••••" />
          </div>

          <button type="submit" className="submit-btn">{current.submit}</button>
        </form>

        <div className="switch-auth">
          {current.hasAccount} <Link to="/login">{current.loginLink}</Link>
        </div>
      </div>
    </div>
  );
}

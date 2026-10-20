import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

export default function Login() {
  // خواندن زبان از localStorage یا پیش‌فرض فارسی
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
      title: "ورود به حساب کاربری",
      desc: "خوش آمدید! لطفاً اطلاعات خود را وارد کنید",
      email: "ایمیل",
      password: "رمز عبور",
      submit: "ورود",
      noAccount: "حساب کاربری ندارید؟",
      registerLink: "ثبت‌نام کنید"
    },
    en: {
      brand: "FinTracker",
      subtitle: "Smart Financial Management • Secure Future",
      title: "Welcome Back",
      desc: "Please enter your credentials to login",
      email: "Email",
      password: "Password",
      submit: "Login",
      noAccount: "Don't have an account?",
      registerLink: "Sign up"
    }
  };

  const current = t[lang];

  return (
    <div className="auth-container" dir={lang === 'fa' ? 'rtl' : 'ltr'}>
      <div className="glass-card">
        <div className="lang-switch">
          <button className={`lang-btn ${lang === 'fa' ? 'active' : ''}`} onClick={() => changeLang('fa')}>فارسی</button>
          <span>|</span>
          <button className={`lang-btn ${lang === 'en' ? 'active' : ''}`} onClick={() => changeLang('en')}>English</button>
        </div>

        <h1 className="brand-title">{current.brand}</h1>
        <p className="brand-subtitle">{current.subtitle}</p>
        
        <h2 style={{ textAlign: 'center', color: '#1f2937', fontSize: '20px', marginBottom: '5px' }}>{current.title}</h2>
        <p style={{ textAlign: 'center', color: '#6b7280', fontSize: '13px', marginBottom: '20px' }}>{current.desc}</p>

        <form onSubmit={(e) => e.preventDefault()}>
          <div className="form-group">
            <label>{current.email}</label>
            <input type="email" placeholder={current.email} />
          </div>

          <div className="form-group">
            <label>{current.password}</label>
            <input type="password" placeholder={current.password} />
          </div>

          <button type="submit" className="submit-btn">{current.submit}</button>
        </form>

        <div className="switch-auth">
          {current.noAccount} <Link to="/register">{current.registerLink}</Link>
        </div>
      </div>
    </div>
  );
}
